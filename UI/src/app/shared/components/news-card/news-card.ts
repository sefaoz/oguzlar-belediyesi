import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NewsItem } from '../../models/news.model';

import { ImageUrlPipe } from '../../pipes/image-url.pipe';

@Component({
  selector: 'app-news-card',
  standalone: true,
  imports: [CommonModule, RouterModule, ImageUrlPipe],
  templateUrl: './news-card.html',
  styleUrl: './news-card.css',
})
export class NewsCard {
  @Input() news!: NewsItem;

}
