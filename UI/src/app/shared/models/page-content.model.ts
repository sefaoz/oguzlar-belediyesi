export interface ContactDetail {
  label: string;
  value: string;
}

export interface PageContentModel {
  title: string;
  subtitle?: string;
  imageUrl?: string;
  paragraphs: string[];
  mapEmbedUrl?: string;
  contactDetails?: ContactDetail[];
}
