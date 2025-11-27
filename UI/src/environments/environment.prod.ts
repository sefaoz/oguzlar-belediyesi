import { EnvironmentConfig } from './environment';

const apiBaseUrl = 'http://localhost:5002/api';

export const environmentProd: EnvironmentConfig = {
  production: true,
  apiBaseUrl,
  newsApiUrl: `${apiBaseUrl}/news`,
  pageContentUrl: `${apiBaseUrl}/pages`,
  announcementApiUrl: `${apiBaseUrl}/announcements`,
  eventApiUrl: `${apiBaseUrl}/events`,
  tenderApiUrl: `${apiBaseUrl}/tenders`
};
