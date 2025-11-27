export interface EnvironmentConfig {
  production: boolean;
  apiBaseUrl: string;
  newsApiUrl: string;
  pageContentUrl: string;
  announcementApiUrl: string;
  eventApiUrl: string;
  tenderApiUrl: string;
}

const apiBaseUrl = 'http://localhost:5002/api';

export const environment: EnvironmentConfig = {
  production: false,
  apiBaseUrl,
  newsApiUrl: `${apiBaseUrl}/news`,
  pageContentUrl: `${apiBaseUrl}/pages`,
  announcementApiUrl: `${apiBaseUrl}/announcements`,
  eventApiUrl: `${apiBaseUrl}/events`,
  tenderApiUrl: `${apiBaseUrl}/tenders`
};
