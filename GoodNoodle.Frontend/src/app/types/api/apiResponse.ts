export type ApiResponse<T> = {
  success: boolean;
  errors?: string[];
  data: T;
};
