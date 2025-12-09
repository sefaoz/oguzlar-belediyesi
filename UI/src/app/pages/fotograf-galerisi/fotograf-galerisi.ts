import { Component, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { GalleryService } from '../../shared/services/gallery.service';
import { GalleryFolder, GalleryImage } from '../../shared/models/gallery.model';
import { SeoService } from '../../shared/services/seo.service';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-fotograf-galerisi',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ImageUrlPipe],
  templateUrl: './fotograf-galerisi.html',
  styleUrls: ['./fotograf-galerisi.css']
})
export class FotografGalerisi implements OnInit {
  folders: GalleryFolder[] = [];
  selectedFolder: GalleryFolder | null = null;
  images: GalleryImage[] = [];

  constructor(
    private readonly galleryService: GalleryService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly seoService: SeoService
  ) { }

  ngOnInit() {
    this.galleryService.getFolders().subscribe(folders => {
      this.folders = folders;
    });

    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.loadFolderBySlug(slug);
      } else {
        this.selectedFolder = null;
        this.images = [];
        this.updateGallerySeo();
      }
    });
  }

  loadFolderBySlug(slug: string) {
    this.galleryService.getFolderBySlug(slug).subscribe(folder => {
      if (folder) {
        this.selectedFolder = folder;
        this.galleryService.getImages(folder.id).subscribe(images => {
          this.images = images;
        });

        // Update SEO for specific folder
        this.seoService.updateSeo({
          title: folder.title,
          description: `${folder.title} fotoğraf albümü. Oğuzlar Belediyesi etkinlik ve proje fotoğrafları.`,
          image: folder.coverImage,
          slug: `fotograf-galerisi/${folder.slug}`
        });
      }
    });
  }

  private updateGallerySeo() {
    this.seoService.updateSeo({
      title: 'Fotoğraf Galerisi',
      description: 'Oğuzlar Belediyesi fotoğraf galerisi. Etkinlikler, projeler ve ilçemizden kareler.',
      slug: 'fotograf-galerisi'
    });
  }

  get breadcrumbs(): BreadcrumbStep[] {
    const steps: BreadcrumbStep[] = [
      { label: 'Anasayfa', url: '/' },
      { label: "Oğuzlar'ı Keşfet" },
      { label: 'Fotoğraf Galerisi', url: '/fotograf-galerisi', active: !this.selectedFolder }
    ];

    if (this.selectedFolder) {
      steps.push({ label: this.selectedFolder.title, active: true });
    }

    return steps;
  }

  selectFolder(folder: GalleryFolder) {
    this.router.navigate(['/fotograf-galerisi', folder.slug]);
    window.scrollTo(0, 0);
  }

  goBack() {
    this.router.navigate(['/fotograf-galerisi']);
    window.scrollTo(0, 0);
  }

  selectedImage: GalleryImage | null = null;

  openPreview(image: GalleryImage) {
    this.selectedImage = image;
    document.body.style.overflow = 'hidden'; // Prevent background scrolling
  }

  closePreview() {
    this.selectedImage = null;
    document.body.style.overflow = 'auto'; // Restore scrolling
  }

  nextImage(event?: Event) {
    event?.stopPropagation();
    if (!this.selectedImage || this.images.length === 0) return;

    const currentIndex = this.images.findIndex(img => img.id === this.selectedImage!.id);
    const nextIndex = (currentIndex + 1) % this.images.length;
    this.selectedImage = this.images[nextIndex];
  }

  prevImage(event?: Event) {
    event?.stopPropagation();
    if (!this.selectedImage || this.images.length === 0) return;

    const currentIndex = this.images.findIndex(img => img.id === this.selectedImage!.id);
    const prevIndex = (currentIndex - 1 + this.images.length) % this.images.length;
    this.selectedImage = this.images[prevIndex];
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (!this.selectedImage) return;

    if (event.key === 'Escape') {
      this.closePreview();
    } else if (event.key === 'ArrowRight') {
      this.nextImage();
    } else if (event.key === 'ArrowLeft') {
      this.prevImage();
    }
  }
}
