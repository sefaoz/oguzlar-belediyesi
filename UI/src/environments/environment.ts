export interface EnvironmentConfig {
  production: boolean;
  apiBaseUrl: string;
  newsApiUrl: string;
  pageContentUrl: string;
  galleryApiUrl: string;
  announcementApiUrl: string;
  eventApiUrl: string;
  tenderApiUrl: string;
  meclisApiUrl: string;
  kvkkApiUrl: string;
  vehicleApiUrl: string;
  unitApiUrl: string;
  useMockData: boolean;
}

const apiBaseUrl = 'http://localhost:5002/api';

export const environment: EnvironmentConfig = {
  production: false,
  apiBaseUrl,
  newsApiUrl: `${apiBaseUrl}/news`,
  pageContentUrl: `${apiBaseUrl}/pages`,
  galleryApiUrl: `${apiBaseUrl}/gallery`,
  announcementApiUrl: `${apiBaseUrl}/announcements`,
  eventApiUrl: `${apiBaseUrl}/events`,
  tenderApiUrl: `${apiBaseUrl}/tenders`,
  meclisApiUrl: `${apiBaseUrl}/meclis`,
  kvkkApiUrl: `${apiBaseUrl}/kvkk`,
  vehicleApiUrl: `${apiBaseUrl}/vehicles`,
  unitApiUrl: `${apiBaseUrl}/units`,
  useMockData: false
};
