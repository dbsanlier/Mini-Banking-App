import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getMusteriById } from '../services/musteriService';
import { getHesaplarByMusteriId } from '../services/hesapService';
import './MusteriDetay.css';

const formatBakiye = (deger) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(deger);

function MusteriDetay() {
  const { id } = useParams();
  const [musteri, setMusteri] = useState(null);
  const [hesaplar, setHesaplar] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    Promise.all([getMusteriById(id), getHesaplarByMusteriId(id)])
      .then(([musteriData, hesapData]) => {
        setMusteri(musteriData);
        setHesaplar(hesapData);
      })
      .catch(() => setError('Müşteri bilgileri yüklenemedi.'))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <div className="state-message">Yükleniyor...</div>;
  if (error) return <div className="state-message state-message--error">{error}</div>;
  if (!musteri) return null;

  return (
    <div className="musteri-detay">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Müşteri Detayı</div>
          <h1 className="page-header__title">{musteri.ad} {musteri.soyad}</h1>
        </div>
        <Link to={`/musteriler/${id}/duzenle`} className="btn btn--secondary">
          Bilgileri Düzenle
        </Link>
      </div>

      <div className="info-grid">
        <div className="info-item">
          <div className="info-item__label">TC Kimlik No</div>
          <div className="info-item__value info-item__value--mono">{musteri.tcKimlikNo}</div>
        </div>
        <div className="info-item">
          <div className="info-item__label">Telefon</div>
          <div className="info-item__value">{musteri.telefon}</div>
        </div>
        <div className="info-item">
          <div className="info-item__label">E-posta</div>
          <div className="info-item__value">{musteri.email}</div>
        </div>
        <div className="info-item">
          <div className="info-item__label">Kayıt Tarihi</div>
          <div className="info-item__value">
            {new Date(musteri.olusturulmaTarihi).toLocaleDateString('tr-TR')}
          </div>
        </div>
      </div>

      <div className="section-header">
        <h2 className="section-header__title">Hesaplar</h2>
        <Link to={`/hesaplar/yeni?musteriId=${id}`} className="btn btn--primary">
          + Hesap Aç
        </Link>
      </div>

      {hesaplar.length === 0 ? (
        <div className="empty-state">
          <p>Bu müşteriye ait hesap bulunmuyor.</p>
        </div>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Hesap No</th>
              <th>IBAN</th>
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

export default MusteriDetay;