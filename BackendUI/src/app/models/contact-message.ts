export interface ContactMessage {
    id: string;
    name: string;
    email: string;
    phone: string;
    message: string;
    createdAt: string;
    isRead: boolean;
    messageType: 'Contact' | 'MayorMessage';
    kvkkAccepted?: boolean;
    ipAddress?: string;
}
