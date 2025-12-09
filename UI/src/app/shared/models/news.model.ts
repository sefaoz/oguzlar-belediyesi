export interface NewsItem {
  image: string;
  date: string;
  title: string;
  description: string;
  slug: string;
  link?: string;
  photos?: string[];
  viewCount?: number;
  tags?: string[];
}
