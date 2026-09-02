export interface RegisterRequest {
  login: string;
  email: string;
  password: string;
}

export interface LoginRequest{
  email: string;
  password: string;
}

export interface AuthenticationResponse {
  id: string;
  login: string;
  email: string;
  token: string;
}
