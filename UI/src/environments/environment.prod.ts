import { EnvironmentConfig } from './environment';

const apiBaseUrl = 'http://localhost:5002/api';
const imageBaseUrl = 'http://localhost:5002';

export const environmentProd: EnvironmentConfig = {
  production: true,
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
  useMockData: true
};
