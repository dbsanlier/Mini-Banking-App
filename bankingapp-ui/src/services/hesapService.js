import api from './api';

export const getHesaplar = async () => {
  const response = await api.get('/hesap');
  return response.data;
};

export const getHesapById = async (id) => {
  const response = await api.get(`/hesap/${id}`);
  return response.data;
};

export const getHesaplarByMusteriId = async (musteriId) => {
  const response = await api.get(`/hesap/musteri/${musteriId}`);
  return response.data;
};

export const createHesap = async (hesapData) => {
  const response = await api.post('/hesap', hesapData);
  return response.data;
};

export const kapatHesap = async (id) => {
  await api.put(`/hesap/${id}/kapat`);
};