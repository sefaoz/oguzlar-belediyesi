export interface LoginRequest {
    username: string;
    password: string;
    rememberMe?: boolean;
}

export interface LoginResponse {
    token: string;
    refreshToken: string;
    user: UserInfo;
    expiresAt: Date;
}

export interface UserInfo {
    id: string;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
}

export interface RefreshTokenRequest {
    refreshToken: string;
}

export interface AuthState {
    isAuthenticated: boolean;
    user: UserInfo | null;
    token: string | null;
    refreshToken: string | null;
}