import api from './api';

export const getMusteriler = async () => {
  const response = await api.get('/musteri');
  return response.data;
};

export const getMusteriById = async (id) => {
  const response = await api.get(`/musteri/${id}`);
  return response.data;
};

export const createMusteri = async (musteriData) => {
  const response = await api.post('/musteri', musteriData);
  return response.data;
};

export const updateMusteri = async (id, musteriData) => {
  const response = await api.put(`/musteri/${id}`, musteriData);
  return response.data;
};

export const deleteMusteri = async (id) => {
  await api.delete(`/musteri/${id}`);
};

export const searchMusteri = async (term) => {
  const response = await api.get(`/musteri/search?term=${term}`);
  return response.data;
};