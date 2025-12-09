export interface EnvironmentConfig {
  production: boolean;
  apiBaseUrl: string;
  imageBaseUrl: string;
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
  sliderApiUrl: string;
  useMockData: boolean;
}

const apiBaseUrl = 'http://localhost:5002/api';
const imageBaseUrl = 'http://localhost:5002';

export const environment: EnvironmentConfig = {
  production: false,
  apiBaseUrl,
  imageBaseUrl,
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
  sliderApiUrl: `${apiBaseUrl}/sliders`,
  useMockData: false
};
