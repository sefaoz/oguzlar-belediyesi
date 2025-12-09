export interface Announcement {
    id: string;
    title: string;
    summary?: string;
    content: string;
    image: string;
    date: string;
    slug: string;
    tags?: string[];
    category?: string;
    photos?: string[];
    viewCount?: number;
}
