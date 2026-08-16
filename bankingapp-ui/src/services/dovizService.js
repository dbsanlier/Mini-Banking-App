import api from './api';
 
export const getDovizKurlari = async () => {
  const response = await api.get('/doviz/kurlar');
  return response.data;
};