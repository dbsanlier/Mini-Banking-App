import api from './api';

export const getIslemlerByHesapId = async (hesapId) => {
  const response = await api.get(`/islem/hesap/${hesapId}`);
  return response.data;
};

export const paraYatir = async (data) => {
  const response = await api.post('/islem/para-yatir', data);
  return response.data;
};

export const paraCek = async (data) => {
  const response = await api.post('/islem/para-cek', data);
  return response.data;
};

export const transferYap = async (data) => {
  const response = await api.post('/islem/transfer', data);
  return response.data;
};
