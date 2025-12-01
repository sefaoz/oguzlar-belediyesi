export interface CouncilDocument {
  id: number;
  title: string;
  type: 'Rapor' | 'Karar' | 'Liste';
  date: string;
  description?: string;
  fileUrl?: string;
}
