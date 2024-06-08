import React from "react";
import ReactDOM from "react-dom/client";
import { SnackbarProvider } from "notistack";

import "./index.css";
import "bootswatch/dist/morph/bootstrap.css";
import "bootstrap-icons/font/bootstrap-icons.css";
import { RouterProvider } from "react-router";
import { router } from "./router";
import { SnackbarUtilsConfigurator } from "./utils/SnackbarUtils";
import { Provider } from "react-redux";
import { store } from "./redux/store";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

root.render(
  <React.StrictMode>
    <Provider store={store}>
      <SnackbarProvider
        maxSnack={4}
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
      >
        <SnackbarUtilsConfigurator />
        <RouterProvider router={router}></RouterProvider>
      </SnackbarProvider>
    </Provider>
  </React.StrictMode>
);
