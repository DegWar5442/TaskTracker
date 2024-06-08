import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { UserDto } from '../../api/auth/models/login';
import { cleanAcceesToken, getAccessToken, setAccessToken } from '../../common/localStorage';

interface AuthState {
  token: string | null;
  user: UserDto | null;
}

const initialState: AuthState = {
  token: getAccessToken(),
  user: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setAuth: (state, action: PayloadAction<{ token: string; user: UserDto }>) => {
      state.token = action.payload.token;
      state.user = action.payload.user;
      setAccessToken(action.payload.token);
    },
    clearAuth: (state) => {
      state.token = null;
      state.user = null;
      cleanAcceesToken();
    },
  },
});

export const { setAuth, clearAuth } = authSlice.actions;

export default authSlice.reducer;
