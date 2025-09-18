import { jwtDecode } from "jwt-decode";

export interface JwtPayload {
  sub: string; // user.Id
  unique_name: string; // username
  name: string; // username
  role: string; // role
  exp: number; // expiration timestamp
  iss?: string; // issuer (optional)
  aud?: string; // audience (optional)
}

export const getUserFromToken = (token: string): JwtPayload | null => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    return decoded;
  } catch {
    return null;
  }
};
