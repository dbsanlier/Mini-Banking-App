import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getHesapById, kapatHesap } from '../services/hesapService';
import { getIslemlerByHesapId, paraYatir, paraCek, transferYap } from '../services/islemService';
import './HesapDetay.css';

const formatBakiye = (deger) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(deger);

const islemTipiEtiket = {
  ParaYatirma: 'Para Yatırma',
  ParaCekme: 'Para Çekme',
  Transfer: 'Transfer',
};

function HesapDetay() {
  const { id } = useParams();
  const [hesap, setHesap] = useState(null);
  const [islemler, setIslemler] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [aktifForm, setAktifForm] = useState(null); // 'yatir' | 'cek' | 'transfer' | null
  const [tutar, setTutar] = useState('');
  const [aciklama, setAciklama] = useState('');
  const [aliciIban, setAliciIban] = useState('');
  const [aliciAdSoyad, setAliciAdSoyad] = useState('');
  const [islemHatasi, setIslemHatasi] = useState(null);
  const [islemSuruyor, setIslemSuruyor] = useState(false);

  const fetchData = async () => {
    try {
      const [hesapData, islemData] = await Promise.all([
        getHesapById(id),
        getIslemlerByHesapId(id),
      ]);
      setHesap(hesapData);
      setIslemler(islemData);
    } catch {
      setError('Hesap bilgileri yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const resetForm = () => {
    setAktifForm(null);
    setTutar('');
    setAciklama('');
    setAliciIban('');
    setAliciAdSoyad('');
    setIslemHatasi(null);
  };

  const handleIslemSubmit = async (e) => {
    e.preventDefault();
    setIslemSuruyor(true);
    setIslemHatasi(null);

    try {
      if (aktifForm === 'yatir') {
        await paraYatir({ hesapId: Number(id), tutar: Number(tutar), aciklama });
      } else if (aktifForm === 'cek') {
        await paraCek({ hesapId: Number(id), tutar: Number(tutar), aciklama });
      } else if (aktifForm === 'transfer') {
        await transferYap({
          gonderenHesapId: Number(id),
          aliciIban: aliciIban.trim(),
          aliciAdSoyad: aliciAdSoyad.trim(),
          tutar: Number(tutar),
          aciklama,
        });
      }
      resetForm();
      await fetchData();
    } catch (err) {
      const message = err.response?.data?.message || 'İşlem gerçekleştirilemedi.';
      setIslemHatasi(message);
    } finally {
      setIslemSuruyor(false);
    }
  };

  const handleKapat = async () => {
    if (!window.confirm('Bu hesabı kapatmak istediğinize emin misiniz?')) return;
    try {
      await kapatHesap(id);
      await fetchData();
    } catch (err) {
      alert(err.response?.data?.message || 'Hesap kapatılamadı.');
    }
  };

  if (loading) return <div className="state-message">Yükleniyor...</div>;
  if (error) return <div className="state-message state-message--error">{error}</div>;
  if (!hesap) return null;

  const hesapAktif = hesap.durum === 'Aktif';

  return (
    <div className="hesap-detay">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Hesap Detayı</div>
          <h1 className="page-header__title">{hesap.hesapNumarasi}</h1>
        </div>
        {hesapAktif && (
          <button className="btn btn--secondary" onClick={handleKapat}>
            Hesabı Kapat
          </button>
        )}
      </div>

      <div className="balance-card">
        <div className="balance-card__label">Güncel Bakiye</div>
        <div className="balance-card__amount">{formatBakiye(hesap.bakiye)}</div>
        <div className="balance-card__meta">
          <span>{hesap.iban}</span>
          <span className={`badge ${hesapAktif ? 'badge--success' : 'badge--muted'}`}>
            {hesap.durum}
          </span>
        </div>
        <Link to={`/musteriler/${hesap.musteriId}`} className="balance-card__owner">
          Hesap sahibi: {hesap.musteriAdSoyad}
        </Link>
      </div>

      {hesapAktif && (
        <div className="action-bar">
          <button className="btn btn--primary" onClick={() => setAktifForm('yatir')}>
            Para Yatır
          </button>
          <button className="btn btn--secondary" onClick={() => setAktifForm('cek')}>
            Para Çek
          </button>
          <button className="btn btn--secondary" onClick={() => setAktifForm('transfer')}>
            Transfer Yap
          </button>
        </div>
      )}

      {aktifForm && (
        <form className="form-card islem-form-card" onSubmit={handleIslemSubmit}>
          <h3 className="islem-form-card__title">
            {aktifForm === 'yatir' && 'Para Yatır'}
            {aktifForm === 'cek' && 'Para Çek'}
            {aktifForm === 'transfer' && 'Transfer Yap'}
          </h3>

          {islemHatasi && (
            <div className="state-message state-message--error form-error">{islemHatasi}</div>
          )}

          {aktifForm === 'transfer' && (
            <>
              <div className="form-field">
                <label htmlFor="aliciIban">Alıcı IBAN</label>
                <input
                  id="aliciIban"
                  type="text"
                  value={aliciIban}
                  onChange={(e) => setAliciIban(e.target.value.toUpperCase())}
                  placeholder="TR..."
                  maxLength={26}
                  required
                />
              </div>

              <div className="form-field">
                <label htmlFor="aliciAdSoyad">Alıcı Ad Soyad</label>
                <input
                  id="aliciAdSoyad"
                  type="text"
                  value={aliciAdSoyad}
                  onChange={(e) => setAliciAdSoyad(e.target.value)}
                  placeholder="Hesap sahibinin adı soyadı"
                  required
                />
                <span className="form-field__hint">
                  Girilen ad soyad, IBAN sahibiyle eşleşmezse işlem gerçekleşmez.
                </span>
              </div>
            </>
          )}

          <div className="form-field">
            <label htmlFor="tutar">Tutar (₺)</label>
            <input
              id="tutar"
              type="number"
              min="0.01"
              step="0.01"
              value={tutar}
              onChange={(e) => setTutar(e.target.value)}
              required
            />
          </div>

          <div className="form-field">
            <label htmlFor="aciklama">Açıklama (opsiyonel)</label>
            <input
              id="aciklama"
              type="text"
              value={aciklama}
              onChange={(e) => setAciklama(e.target.value)}
            />
          </div>

          <div className="form-actions">
            <button type="submit" className="btn btn--primary" disabled={islemSuruyor}>
              {islemSuruyor ? 'İşleniyor...' : 'Onayla'}
            </button>
            <button type="button" className="btn btn--secondary" onClick={resetForm}>
              Vazgeç
            </button>
          </div>
        </form>
      )}

      <div className="section-header">
        <h2 className="section-header__title">İşlem Geçmişi</h2>
      </div>

      {islemler.length === 0 ? (
        <div className="empty-state">
          <p>Bu hesaba ait işlem bulunmuyor.</p>
        </div>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Tarih</th>
              <th>Tip</th>
              <th>Tutar</th>
              <th>Açıklama</th>
              <th>Karşı Hesap</th>
            </tr>
          </thead>
          <tbody>
            {islemler.map((islem) => {
              const cikan = islem.gonderenHesapNumarasi === hesap.hesapNumarasi && islem.tipi === 'Transfer';
              const karsiHesap = cikan ? islem.aliciHesapNumarasi : islem.gonderenHesapNumarasi;
              return (
                <tr key={islem.id}>
                  <td>{new Date(islem.islemTarihi).toLocaleString('tr-TR')}</td>
                  <td>{islemTipiEtiket[islem.tipi] || islem.tipi}</td>
                  <td className={cikan || islem.tipi === 'ParaCekme' ? 'amount amount--negative' : 'amount amount--positive'}>
                    {cikan || islem.tipi === 'ParaCekme' ? '-' : '+'}
                    {formatBakiye(islem.tutar)}
                  </td>
                  <td>{islem.aciklama || '—'}</td>
                  <td className="data-table__mono">
                    {islem.tipi === 'Transfer' ? karsiHesap : '—'}
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default HesapDetay;
