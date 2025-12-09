export interface Tender {
  id: string;
  title: string;
  tenderDate: string;
  status: string;
  registrationNumber: string;
  slug: string;
  description?: string;
  estimatedValue?: number;
  documentsJson?: string;
  createdDate?: string;
}

export interface TenderDocument {
  Title: string;
  Url: string;
}
