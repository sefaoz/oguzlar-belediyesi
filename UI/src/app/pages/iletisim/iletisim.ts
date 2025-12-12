import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';
import { SeoService } from '../../shared/services/seo.service';
import { ContactMessageService } from '../../shared/services/contact-message.service';

@Component({
  selector: 'app-iletisim',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PageContainerComponent],
  templateUrl: './iletisim.html',
  styleUrl: './iletisim.css',
})
export class IletisimComponent implements OnInit {
  contactForm: FormGroup;
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'İletişim', url: '/iletisim' }
  ];

  safeMapUrl: SafeResourceUrl;

  constructor(
    private fb: FormBuilder,
    private readonly pageContentService: PageContentService,
    private sanitizer: DomSanitizer,
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

    this.safeMapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
      'https://maps.google.com/maps?q=Oğuzlar+Belediyesi+Çorum&t=&z=15&ie=UTF8&iwloc=&output=embed'
    );
  }

  isLoading = false;
  content?: PageContentModel;
  submitSuccess = false;
  submitError = '';

  ngOnInit(): void {
    this.pageContentService.getPageContent('iletisim').subscribe(content => {
      this.content = content;
      if (content?.mapEmbedUrl) {
        if (content.mapEmbedUrl.startsWith('https://www.google.com/maps') || content.mapEmbedUrl.startsWith('https://maps.google.com')) {
          this.safeMapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(content.mapEmbedUrl);
        }
      }
      this.seoService.updateSeo({
        title: 'İletişim',
        description: 'Oğuzlar Belediyesi iletişim bilgileri, adres, telefon ve ulaşım haritası.',
        slug: 'iletisim'
      });
    });
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

      this.contactMessageService.sendContactMessage(request).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.submitSuccess = true;
          alert(response.message || 'Mesajınız başarıyla iletildi. Teşekkür ederiz.');
          this.contactForm.reset();
        },
        error: (error) => {
          this.isLoading = false;
          this.submitError = 'Mesaj gönderilirken bir hata oluştu. Lütfen tekrar deneyin.';
          alert(this.submitError);
        }
      });
    } else {
      this.contactForm.markAllAsTouched();
    }
  }
}

