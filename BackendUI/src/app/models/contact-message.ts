export interface ContactMessage {
    id: number;
    name: string;
    email: string;
    phone: string;
    message: string;
    date: string;
    isRead: boolean;
    type: 'Contact' | 'MayorMessage'; // 'İletişim' or 'Başkana Mesaj'
}
