import { EnvironmentConfig } from './environment';

const apiBaseUrl = 'http://localhost:5002/api';

export const environmentProd: EnvironmentConfig = {
  production: true,
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
  sliderApiUrl: `${apiBaseUrl}/sliders`,
  useMockData: true
};
