export interface EnvironmentConfig {
  production: boolean;
  apiUrl: string;
}

export const environment: EnvironmentConfig = {
  production: false,
  apiUrl: 'http://localhost:5002/api'
};
