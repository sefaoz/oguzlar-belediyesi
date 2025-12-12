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
import { BlockUIModule } from 'primeng/blockui';
import { ProgressBarModule } from 'primeng/progressbar';
import { MessageService, ConfirmationService } from 'primeng/api';
import { NewsService } from '../../services/news.service';
import { News } from '../../models/news';
import { environment } from '../../../environments/environment';

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
    EditorModule,
    BlockUIModule,
    ProgressBarModule
  ],
  providers: [DatePipe],
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
  tagsInput: string = '';

  selectedGalleryFiles: File[] = [];
  selectedGalleryPreviews: string[] = [];
  existingGalleryPhotos: string[] = [];

  isLoading: boolean = false;
  progressValue: number = 0;
  progressInterval: any;

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
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Haberler yüklenemedi.' });
      }
    });
  }

  openNew() {
    this.news = {} as News;
    const today = new Date();
    this.news.date = this.datePipe.transform(today, 'yyyy-MM-dd') || '';

    this.submitted = false;
    this.newsDialog = true;
    this.selectedImage = undefined;
    this.selectedImagePreview = undefined;
    this.originalImageUrl = undefined;
    this.tagsInput = '';

    this.selectedGalleryFiles = [];
    this.selectedGalleryPreviews = [];
    this.existingGalleryPhotos = [];
  }

  editNews(newsItem: News) {
    this.news = { ...newsItem };
    if (this.news.date) {
      this.news.date = this.datePipe.transform(this.news.date, 'yyyy-MM-dd') || '';
    }
    this.originalImageUrl = this.news.image;
    this.selectedImagePreview = undefined;
    this.selectedImage = undefined;
    this.tagsInput = this.news.tags ? this.news.tags.join(', ') : '';

    this.selectedGalleryFiles = [];
    this.selectedGalleryPreviews = [];
    this.existingGalleryPhotos = this.news.photos ? [...this.news.photos] : [];

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
          error: () => {
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

  onGalleryFilesSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      const files: FileList = event.target.files;
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        this.selectedGalleryFiles.push(file);

        const reader = new FileReader();
        reader.onload = (e) => {
          this.selectedGalleryPreviews.push(e.target?.result as string);
        };
        reader.readAsDataURL(file);
      }
    }
  }

  removeGalleryFile(index: number) {
    this.selectedGalleryFiles.splice(index, 1);
    this.selectedGalleryPreviews.splice(index, 1);
  }

  removeExistingGalleryPhoto(index: number) {
    this.existingGalleryPhotos.splice(index, 1);
    this.news.photos = this.existingGalleryPhotos;
  }

  saveNews() {
    this.submitted = true;

    if (this.news.title?.trim()) {
      this.isLoading = true;
      this.progressValue = 0;
      this.startTimer();

      if (this.tagsInput) {
        this.news.tags = this.tagsInput.split(',').map(tag => tag.trim()).filter(tag => tag !== '');
      } else {
        this.news.tags = [];
      }

      const finalizeCallback = () => {
        this.isLoading = false;
        this.stopTimer();
      };

      if (this.news.id) {
        const { id, ...newsData } = this.news;
        this.newsService.update(id, newsData, this.selectedImage, this.selectedGalleryFiles).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Haber güncellendi.', life: 3000 });
            this.getNews();
            this.newsDialog = false;
            this.news = {} as News;
          },
          error: (error) => {
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Haber güncellenirken hata oluştu.' });
          },
          complete: finalizeCallback
        });
      } else {
        this.newsService.create(this.news, this.selectedImage, this.selectedGalleryFiles).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Haber oluşturuldu.', life: 3000 });
            this.getNews();
            this.newsDialog = false;
            this.news = {} as News;
          },
          error: (error) => {
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Haber oluşturulurken hata oluştu.' });
          },
          complete: finalizeCallback
        });
      }
    }
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    if (!isNaN(date.getTime())) {
      return this.datePipe.transform(date, 'dd.MM.yyyy') || '';
    }
    return dateString;
  }

  getImageUrl(url: string): string {
    if (!url) return '';
    if (url.startsWith('http')) return url;
    return `${environment.imageBaseUrl}${url}`;
  }

  startTimer() {
    this.progressInterval = setInterval(() => {
      this.progressValue += 1;
      if (this.progressValue >= 90) {
      }
    }, 100);
  }

  stopTimer() {
    if (this.progressInterval) {
      clearInterval(this.progressInterval);
      this.progressInterval = null;
    }
  }
}
