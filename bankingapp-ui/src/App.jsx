import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import AnaSayfa from './pages/AnaSayfa';
import MusteriListesi from './pages/MusteriListesi';
import MusteriForm from './pages/MusteriForm';
import MusteriDetay from './pages/MusteriDetay';
import HesapListesi from './pages/HesapListesi';
import HesapForm from './pages/HesapForm';
import HesapDetay from './pages/HesapDetay';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<AnaSayfa />} />
          <Route path="musteriler" element={<MusteriListesi />} />
          <Route path="musteriler/yeni" element={<MusteriForm />} />
          <Route path="musteriler/:id" element={<MusteriDetay />} />
          <Route path="musteriler/:id/duzenle" element={<MusteriForm />} />
          <Route path="hesaplar" element={<HesapListesi />} />
          <Route path="hesaplar/yeni" element={<HesapForm />} />
          <Route path="hesaplar/:id" element={<HesapDetay />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;