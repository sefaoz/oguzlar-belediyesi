import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Table, TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { FileUploadModule } from 'primeng/fileupload';
import { ImageModule } from 'primeng/image';
import { EditorModule } from 'primeng/editor';
import { MessageService, ConfirmationService } from 'primeng/api';
import { NewsService } from '../../services/news.service';
import { News } from '../../models/news';

@Component({
  selector: 'app-news',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    DialogModule,
    ButtonModule,
    RippleModule,
    ToastModule,
    ToolbarModule,
    ConfirmDialogModule,
    InputTextModule,
    TextareaModule,
    FileUploadModule,
    ImageModule,
    EditorModule
  ],
  providers: [MessageService, ConfirmationService, DatePipe],
  templateUrl: './news.html',
  styleUrl: './news.scss',
})
export class NewsComponent implements OnInit {
  newsDialog: boolean = false;
  newsList: News[] = [];
  news: News = {} as News;
  submitted: boolean = false;
  selectedImage: File | undefined;
  selectedImagePreview: string | undefined;
  originalImageUrl: string | undefined;

  constructor(
    private newsService: NewsService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.getNews();
  }

  getNews() {
    this.newsService.getAll().subscribe({
      next: (data) => {
        this.newsList = data;
      },
      error: (error) => {
        console.error('Haberler yüklenirken hata oluştu:', error);
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Haberler yüklenemedi.' });
      }
    });
  }

  openNew() {
    this.news = {} as News;
    // Varsayılan tarih olarak bugünü ata
    const today = new Date();
    this.news.date = this.datePipe.transform(today, 'yyyy-MM-dd') || '';

    this.submitted = false;
    this.newsDialog = true;
    this.selectedImage = undefined;
    this.selectedImagePreview = undefined;
    this.originalImageUrl = undefined;
  }

  editNews(newsItem: News) {
    this.news = { ...newsItem };
    this.originalImageUrl = this.news.image;
    this.selectedImagePreview = undefined;
    this.selectedImage = undefined;
    this.newsDialog = true;
  }

  deleteNews(newsItem: News) {
    this.confirmationService.confirm({
      message: '"' + newsItem.title + '" başlıklı haberi silmek istediğinize emin misiniz?',
      header: 'Onay',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Evet',
      rejectLabel: 'Hayır',
      accept: () => {
        this.newsService.delete(newsItem.id).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Haber silindi.', life: 3000 });
            this.getNews();
          },
          error: (error) => {
            console.error('Silme hatası:', error);
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Haber silinirken hata oluştu.' });
          }
        });
      }
    });
  }

  hideDialog() {
    this.newsDialog = false;
    this.submitted = false;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedImage = file;
      const reader = new FileReader();
      reader.onload = (e) => {
        this.selectedImagePreview = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  saveNews() {
    this.submitted = true;

    if (this.news.title?.trim()) {
      // Slug oluşturma (basit versiyon)
      if (!this.news.slug) {
        this.news.slug = this.slugify(this.news.title);
      }

      if (this.news.id) {
        // Güncelleme
        const { id, ...newsData } = this.news;
        this.newsService.update(id, newsData, this.selectedImage).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Haber güncellendi.', life: 3000 });
            this.getNews();
            this.newsDialog = false;
            this.news = {} as News;
          },
          error: (error) => {
            console.error('Güncelleme hatası:', error);
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Haber güncellenirken hata oluştu.' });
          }
        });
      } else {
        // Yeni Kayıt
        this.newsService.create(this.news, this.selectedImage).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Haber oluşturuldu.', life: 3000 });
            this.getNews();
            this.newsDialog = false;
            this.news = {} as News;
          },
          error: (error) => {
            console.error('Oluşturma hatası:', error);
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Haber oluşturulurken hata oluştu.' });
          }
        });
      }
    }
  }

  slugify(text: string): string {
    return text.toString().toLowerCase()
      .replace(/\s+/g, '-')           // Replace spaces with -
      .replace(/[^\w\-]+/g, '')       // Remove all non-word chars
      .replace(/\-\-+/g, '-')         // Replace multiple - with single -
      .replace(/^-+/, '')             // Trim - from start of text
      .replace(/-+$/, '');            // Trim - from end of text
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
