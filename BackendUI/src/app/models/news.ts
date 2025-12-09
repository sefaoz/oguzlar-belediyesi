export interface News {
    id: string;
    title: string;
    description: string;
    image: string;
    date: string;
    slug: string;
    tags?: string[];
    photos?: string[];
    viewCount?: number;
}
