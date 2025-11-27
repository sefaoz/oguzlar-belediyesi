import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';

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
    private sanitizer: DomSanitizer
  ) {
    this.contactForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      message: ['', Validators.required],
      kvkk: [false, Validators.requiredTrue]
    });

    // Set default safe URL initially
    this.safeMapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
      'https://maps.google.com/maps?q=Oğuzlar+Belediyesi+Çorum&t=&z=15&ie=UTF8&iwloc=&output=embed'
    );
  }

  isLoading = false;
  content?: PageContentModel;

  ngOnInit(): void {
    this.pageContentService.getPageContent('iletisim').subscribe(content => {
      this.content = content;
      if (content?.mapEmbedUrl) {
        this.safeMapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(content.mapEmbedUrl);
      }
    });
  }

  onSubmit() {
    if (this.contactForm.valid) {
      this.isLoading = true;
      console.log('Form submitted:', this.contactForm.value);

      // Simulate API call
      setTimeout(() => {
        this.isLoading = false;
        alert('Mesajınız başarıyla iletildi. Teşekkür ederiz.');
        this.contactForm.reset();
      }, 2000);
    } else {
      this.contactForm.markAllAsTouched();
    }
  }
}
