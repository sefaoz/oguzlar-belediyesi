export type TenderStatus = 'active' | 'passive' | 'completed';

export interface Tender {
  title: string;
  date: string;
  status: TenderStatus;
  registrationNumber: string;
  slug: string;
  description?: string;
  estimatedValue?: number;
  publishedAt?: string;
}
