import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SiteSettingsService } from '../../services/site-settings.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.html',
  styleUrls: ['./footer.css']
})
export class FooterComponent implements OnInit {
  logoContent: string = '';
  quickLinks: any[] = [];
  socialMedia: any = {};

  constructor(private siteSettingsService: SiteSettingsService) { }

  ngOnInit() {
    this.siteSettingsService.settings$.subscribe(() => {
      this.logoContent = this.siteSettingsService.getSetting('Footer', 'LogoContent') || '';
      this.quickLinks = this.siteSettingsService.getJsonSetting<any[]>('Footer', 'QuickLinks') || [];
      this.socialMedia = this.siteSettingsService.getJsonSetting<any>('Footer', 'SocialMedia') || {};
    });
  }
}
