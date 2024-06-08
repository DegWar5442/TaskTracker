const getLocalStorageItem = (itemKey: string) => {
    let item = '';
    if (typeof window !== 'undefined') {
      item = localStorage.getItem(itemKey) || '';
    }
    return item;
  };
  
  export const getAccessToken = () => getLocalStorageItem('accessToken');
  
  export const cleanAcceesToken = () => localStorage.removeItem('accessToken');

  export const setAccessToken = (accessToken: string) => {
    if (typeof window !== 'undefined') {
      localStorage.setItem('accessToken', accessToken);
    }
  };
  