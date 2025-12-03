import { Component } from '@angular/core';
import { StatsWidget } from './components/statswidget';
import { RecentContentWidget } from './components/recentcontentwidget';

@Component({
    selector: 'app-dashboard',
    imports: [StatsWidget, RecentContentWidget],
    template: `
        <div class="grid grid-cols-12 gap-8">
            <app-stats-widget class="contents" />
            <div class="col-span-12">
                <app-recent-content-widget />
            </div>
        </div>
    `
})
export class Dashboard { }
