import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ContactMessage } from '../models/contact-message';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ContactMessageService {

    constructor(private http: HttpClient) { }

    getMessages(): Observable<ContactMessage[]> {
        // Mock data
        return of([
            {
                id: 1,
                name: 'Ahmet Yılmaz',
                email: 'ahmet@example.com',
                phone: '0555 123 45 67',
                message: 'Parktaki banklar kırık, ilgilenir misiniz?',
                date: '2023-11-20T10:00:00',
                isRead: false,
                type: 'Contact'
            },
            {
                id: 2,
                name: 'Ayşe Demir',
                email: 'ayse@example.com',
                phone: '0505 987 65 43',
                message: 'Başkanım çalışmalarınızdan dolayı teşekkür ederiz. Mahallemize yaptığınız park çok güzel oldu.',
                date: '2023-11-18T14:30:00',
                isRead: true,
                type: 'MayorMessage'
            },
            {
                id: 3,
                name: 'Mehmet Öztürk',
                email: 'mehmet@test.com',
                phone: '0532 111 22 33',
                message: 'Su kesintileri hakkında bilgi almak istiyorum.',
                date: '2023-11-15T09:15:00',
                isRead: true,
                type: 'Contact'
            }
        ]);
    }

    deleteMessage(id: number): Observable<any> {
        // Mock delete
        return of(true);
    }

    markAsRead(id: number): Observable<any> {
        // Mock mark as read
        return of(true);
    }
}
