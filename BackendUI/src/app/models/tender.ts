export interface Tender {
    id: string;
    title: string;
    description: string;
    tenderDate: string;
    registrationNumber: string;
    status: string;
    documentsJson: string;
    slug: string;
    // Helper used in UI, parsed from documentsJson
    documentsList?: TenderDocument[];
}

export interface TenderDocument {
    Title: string;
    Url: string;
}
