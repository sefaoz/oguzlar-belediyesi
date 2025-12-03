import { Component } from '@angular/core';

@Component({
    selector: 'app-empty',
    standalone: true,
    template: ` <div class="card">
        <div class="font-semibold text-xl mb-4">Boş Sayfa</div>
        <p>Bu sayfayı sıfırdan başlamak ve kendi içeriğinizi eklemek için kullanın.</p>
    </div>`
})
export class Empty { }
