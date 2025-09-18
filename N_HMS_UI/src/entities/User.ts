export default interface User {
  id: number;
  username: string;
  role: string;
}

export default interface UserAuth {
  token: string;
  username: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  username: string;
  role: string;
}
