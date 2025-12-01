export interface GalleryFolder {
    id: string;
    title: string;
    coverImage: string;
    imageCount: number;
    date: string;
    slug: string;
}

export interface GalleryImage {
    id: string;
    folderId: string;
    url: string;
    thumbnailUrl: string;
    title?: string;
}
