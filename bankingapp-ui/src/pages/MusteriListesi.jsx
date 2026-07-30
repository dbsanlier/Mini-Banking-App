import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getMusteriler, searchMusteri, deleteMusteri } from '../services/musteriService';
import './MusteriListesi.css';

function MusteriListesi() {
  const [musteriler, setMusteriler] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchMusteriler = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getMusteriler();
      setMusteriler(data);
    } catch (err) {
      setError('Müşteriler yüklenemedi. Backend çalışıyor mu kontrol edin.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMusteriler();
  }, []);

  const handleSearch = async (e) => {
    e.preventDefault();
    if (!searchTerm.trim()) {
      fetchMusteriler();
      return;
    }
    setLoading(true);
    try {
      const data = await searchMusteri(searchTerm);
      setMusteriler(data);
    } catch (err) {
      setError('Arama sırasında bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id, adSoyad) => {
    if (!window.confirm(`${adSoyad} adlı müşteriyi silmek istediğinize emin misiniz?`)) {
      return;
    }
    try {
      await deleteMusteri(id);
      setMusteriler((prev) => prev.filter((m) => m.id !== id));
    } catch (err) {
      alert('Müşteri silinemedi.');
    }
  };

  return (
    <div className="musteri-listesi">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Müşteri Yönetimi</div>
          <h1 className="page-header__title">Müşteriler</h1>
        </div>
        <Link to="/musteriler/yeni" className="btn btn--primary">
          + Yeni Müşteri
        </Link>
      </div>

      <form className="search-bar" onSubmit={handleSearch}>
        <input
          type="text"
          placeholder="Ad, soyad, TC kimlik no veya e-posta ile ara..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <button type="submit" className="btn btn--secondary">Ara</button>
      </form>

      {loading && <div className="state-message">Yükleniyor...</div>}
      {error && <div className="state-message state-message--error">{error}</div>}

      {!loading && !error && musteriler.length === 0 && (
        <div className="empty-state">
          <p>Henüz kayıtlı müşteri yok.</p>
          <Link to="/musteriler/yeni" className="btn btn--primary">İlk müşteriyi ekle</Link>
        </div>
      )}

      {!loading && !error && musteriler.length > 0 && (
        <table className="data-table">
          <thead>
            <tr>
              <th>Ad Soyad</th>
              <th>TC Kimlik No</th>
              <th>Telefon</th>
              <th>E-posta</th>
              <th>Kayıt Tarihi</th>
              <th aria-label="İşlemler"></th>
            </tr>
          </thead>
          <tbody>
            {musteriler.map((musteri) => (
              <tr key={musteri.id}>
                <td className="data-table__primary">
                  <Link to={`/musteriler/${musteri.id}`}>
                    {musteri.ad} {musteri.soyad}
                  </Link>
                </td>
                <td className="data-table__mono">{musteri.tcKimlikNo}</td>
                <td>{musteri.telefon}</td>
                <td>{musteri.email}</td>
                <td>{new Date(musteri.olusturulmaTarihi).toLocaleDateString('tr-TR')}</td>
                <td className="data-table__actions">
                  <Link to={`/musteriler/${musteri.id}/duzenle`} className="link-action">
                    Düzenle
                  </Link>
                  <button
                    className="link-action link-action--danger"
                    onClick={() => handleDelete(musteri.id, `${musteri.ad} ${musteri.soyad}`)}
                  >
                    Sil
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default MusteriListesi;