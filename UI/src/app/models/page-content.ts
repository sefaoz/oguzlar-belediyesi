export interface ContactDetail {
    label: string;
    value: string;
}

export interface PageContent {
    id: string;
    key: string;
    title: string;
    subtitle: string;
    paragraphs: string[];
    imageUrl?: string;
    mapEmbedUrl?: string;
    contactDetails?: ContactDetail[];
}
