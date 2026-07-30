import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getMusteriler } from '../services/musteriService';
import { getHesaplar } from '../services/hesapService';
import './AnaSayfa.css';

const formatBakiye = (deger) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(deger);

function AnaSayfa() {
  const [musteriSayisi, setMusteriSayisi] = useState(0);
  const [hesaplar, setHesaplar] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    Promise.all([getMusteriler(), getHesaplar()])
      .then(([musteriler, hesapData]) => {
        setMusteriSayisi(musteriler.length);
        setHesaplar(hesapData);
      })
      .catch(() => setError('Özet bilgiler yüklenemedi. Backend çalışıyor mu kontrol edin.'))
      .finally(() => setLoading(false));
  }, []);

  const aktifHesaplar = hesaplar.filter((h) => h.durum === 'Aktif');
  const toplamBakiye = aktifHesaplar.reduce((sum, h) => sum + h.bakiye, 0);
  const sonHesaplar = [...hesaplar]
    .sort((a, b) => new Date(b.olusturulmaTarihi) - new Date(a.olusturulmaTarihi))
    .slice(0, 5);

  if (loading) return <div className="state-message">Yükleniyor...</div>;
  if (error) return <div className="state-message state-message--error">{error}</div>;

  return (
    <div className="ana-sayfa">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Genel Bakış</div>
          <h1 className="page-header__title">Ana Sayfa</h1>
        </div>
      </div>

      <div className="stat-grid">
        <div className="stat-card">
          <div className="stat-card__label">Toplam Müşteri</div>
          <div className="stat-card__value">{musteriSayisi}</div>
        </div>
        <div className="stat-card">
          <div className="stat-card__label">Toplam Hesap</div>
          <div className="stat-card__value">{hesaplar.length}</div>
        </div>
        <div className="stat-card">
          <div className="stat-card__label">Aktif Hesap</div>
          <div className="stat-card__value">{aktifHesaplar.length}</div>
        </div>
        <div className="stat-card stat-card--accent">
          <div className="stat-card__label">Toplam Bakiye (Aktif Hesaplar)</div>
          <div className="stat-card__value">{formatBakiye(toplamBakiye)}</div>
        </div>
      </div>

      <div className="section-header">
        <h2 className="section-header__title">Son Açılan Hesaplar</h2>
        <Link to="/hesaplar" className="link-action">Tümünü Gör</Link>
      </div>

      {sonHesaplar.length === 0 ? (
        <div className="empty-state">
          <p>Henüz hesap yok.</p>
          <Link to="/hesaplar/yeni" className="btn btn--primary">İlk hesabı aç</Link>
        </div>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Hesap No</th>
              <th>Müşteri</th>
              <th>Bakiye</th>
              <th>Durum</th>
            </tr>
          </thead>
          <tbody>
            {sonHesaplar.map((hesap) => (
              <tr key={hesap.id}>
                <td className="data-table__mono">
                  <Link to={`/hesaplar/${hesap.id}`}>{hesap.hesapNumarasi}</Link>
                </td>
                <td>{hesap.musteriAdSoyad}</td>
                <td>{formatBakiye(hesap.bakiye)}</td>
                <td>
                  <span className={`badge ${hesap.durum === 'Aktif' ? 'badge--success' : 'badge--muted'}`}>
                    {hesap.durum}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default AnaSayfa;
