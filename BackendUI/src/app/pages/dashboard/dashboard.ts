import { Component } from '@angular/core';
import { StatsWidget } from './components/statswidget';
import { IncomingMessagesWidget } from './components/incomingmessageswidget';

@Component({
    selector: 'app-dashboard',
    imports: [StatsWidget, IncomingMessagesWidget],
    template: `
        <div class="grid grid-cols-12 gap-8">
            <app-stats-widget class="contents" />
            <div class="col-span-12">
                <app-incoming-messages-widget />
            </div>
        </div>
    `
})
export class Dashboard { }
