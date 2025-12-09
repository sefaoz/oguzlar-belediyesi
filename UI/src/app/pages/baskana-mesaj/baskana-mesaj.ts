import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';
import { SeoService } from '../../shared/services/seo.service';
import { ContactMessageService } from '../../shared/services/contact-message.service';

@Component({
  selector: 'app-baskana-mesaj',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PageContainerComponent, ImageUrlPipe],
  templateUrl: './baskana-mesaj.html',
  styleUrl: './baskana-mesaj.css',
})
export class BaskanaMesaj implements OnInit {
  contactForm: FormGroup;
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Başkanımız' },
    { label: 'Başkana Mesaj Yaz', url: '/baskana-mesaj' }
  ];
  isLoading = false;
  content?: PageContentModel;
  submitSuccess = false;
  submitError = '';

  constructor(
    private fb: FormBuilder,
    private readonly pageContentService: PageContentService,
    private readonly seoService: SeoService,
    private readonly contactMessageService: ContactMessageService
  ) {
    this.contactForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      message: ['', Validators.required],
      kvkk: [false, Validators.requiredTrue]
    });
  }

  ngOnInit(): void {
    this.pageContentService.getPageContent('baskana-mesaj').subscribe(content => {
      this.content = content;
      if (this.content?.paragraphs) {
        this.content.paragraphs = this.content.paragraphs.map(p => this.decodeHtml(p));
      }
      this.seoService.updateSeo({
        title: 'Başkana Mesaj',
        description: 'Oğuzlar Belediye Başkanı\'na doğrudan mesaj, istek ve şikayetlerinizi iletin.',
        slug: 'baskana-mesaj'
      });
    });
  }

  private decodeHtml(html: string): string {
    const doc = new DOMParser().parseFromString(html, 'text/html');
    return doc.documentElement.textContent || '';
  }

  onSubmit() {
    if (this.contactForm.valid) {
      this.isLoading = true;
      this.submitError = '';
      this.submitSuccess = false;

      const formValue = this.contactForm.value;
      const request = {
        name: formValue.name,
        email: formValue.email,
        phone: formValue.phone,
        message: formValue.message,
        kvkkAccepted: formValue.kvkk
      };

      this.contactMessageService.sendMayorMessage(request).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.submitSuccess = true;
          alert(response.message || 'Mesajınız Başkana iletilmek üzere kaydedildi. Teşekkür ederiz.');
          this.contactForm.reset();
        },
        error: (error) => {
          this.isLoading = false;
          this.submitError = 'Mesaj gönderilirken bir hata oluştu. Lütfen tekrar deneyin.';
          console.error('Form gönderme hatası:', error);
          alert(this.submitError);
        }
      });
    } else {
      this.contactForm.markAllAsTouched();
    }
  }
}

