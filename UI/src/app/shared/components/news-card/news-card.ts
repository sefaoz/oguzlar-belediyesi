import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface NewsItem {
  image: string;
  date: string;
  title: string;
  description: string;
  link: string;
}

@Component({
  selector: 'app-news-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './news-card.html',
  styleUrl: './news-card.css',
})
export class NewsCard {
  @Input() news!: NewsItem;
}
