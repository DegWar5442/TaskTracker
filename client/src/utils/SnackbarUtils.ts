import { useSnackbar, VariantType, SnackbarMessage } from "notistack";
import React from "react";

let useSnackbarRef: {
  enqueueSnackbar: (message: SnackbarMessage, options?: any) => void;
};
export const SnackbarUtilsConfigurator: React.FC = () => {
  useSnackbarRef = useSnackbar();
  return null;
};

const SnackbarUtils = {
  success(msg: string) {
    this.toast(msg, "success");
  },
  warning(msg: string) {
    this.toast(msg, "warning");
  },
  info(msg: string) {
    this.toast(msg, "info");
  },
  error(msg: string) {
    this.toast(msg, "error");
  },
  toast(msg: string, variant: VariantType = "default") {
    if (useSnackbarRef) {
      useSnackbarRef.enqueueSnackbar(msg, { variant });
    } else {
      console.error(
        "useSnackbarRef is not defined. Did you forget to include SnackbarUtilsConfigurator in your component tree?"
      );
    }
  },
};

export default SnackbarUtils;
