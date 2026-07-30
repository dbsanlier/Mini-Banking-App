import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getHesaplar } from '../services/hesapService';
import './HesapListesi.css';

const formatBakiye = (deger) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(deger);

function HesapListesi() {
  const [hesaplar, setHesaplar] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    getHesaplar()
      .then(setHesaplar)
      .catch(() => setError('Hesaplar yüklenemedi. Backend çalışıyor mu kontrol edin.'))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="hesap-listesi">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Hesap Yönetimi</div>
          <h1 className="page-header__title">Hesaplar</h1>
        </div>
        <Link to="/hesaplar/yeni" className="btn btn--primary">
          + Hesap Aç
        </Link>
      </div>

      {loading && <div className="state-message">Yükleniyor...</div>}
      {error && <div className="state-message state-message--error">{error}</div>}

      {!loading && !error && hesaplar.length === 0 && (
        <div className="empty-state">
          <p>Henüz açılmış hesap yok.</p>
          <Link to="/hesaplar/yeni" className="btn btn--primary">İlk hesabı aç</Link>
        </div>
      )}

      {!loading && !error && hesaplar.length > 0 && (
        <table className="data-table">
          <thead>
            <tr>
              <th>Hesap No</th>
              <th>IBAN</th>
              <th>Müşteri</th>
              <th>Bakiye</th>
              <th>Durum</th>
              <th aria-label="İşlemler"></th>
            </tr>
          </thead>
          <tbody>
            {hesaplar.map((hesap) => (
              <tr key={hesap.id}>
                <td className="data-table__mono">{hesap.hesapNumarasi}</td>
                <td className="data-table__mono">{hesap.iban}</td>
                <td className="data-table__primary">
                  <Link to={`/musteriler/${hesap.musteriId}`}>{hesap.musteriAdSoyad}</Link>
                </td>
                <td>{formatBakiye(hesap.bakiye)}</td>
                <td>
                  <span className={`badge ${hesap.durum === 'Aktif' ? 'badge--success' : 'badge--muted'}`}>
                    {hesap.durum}
                  </span>
                </td>
                <td className="data-table__actions">
                  <Link to={`/hesaplar/${hesap.id}`} className="link-action">
                    Detay
                  </Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default HesapListesi;