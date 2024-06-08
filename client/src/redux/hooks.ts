import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import type { AppDispatch, RootState } from "./store";
import { useEffect } from "react";
import { setAuth } from "./slices/authSlice";
import { me } from "../api/auth/authApi";

export const useAppDispatch = () => useDispatch<AppDispatch>();

export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

const useFetchUser = () => {
  const dispatch = useAppDispatch();
  const { token, user } = useAppSelector((state) => state.auth);

  useEffect(() => {
    const fetchUser = async () => {
      if (!user && token) {
        try {
          const userMe = await me();
          if (userMe) {
            dispatch(setAuth({ token: userMe.token, user: userMe.user }));
          }
        } catch (error) {
          console.error("Failed to fetch user", error);
        }
      }
    };

    fetchUser();
  }, [token, user, dispatch]);
};

export default useFetchUser;
