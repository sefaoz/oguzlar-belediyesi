import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';

@Component({
  selector: 'app-baskana-mesaj',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PageContainerComponent],
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

  constructor(
    private fb: FormBuilder,
    private readonly pageContentService: PageContentService
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
