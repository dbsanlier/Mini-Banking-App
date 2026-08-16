import { useEffect, useState } from 'react';
import { getDovizKurlari } from '../services/dovizService';
import './YatirimSayfasi.css';

const formatTl = (deger) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(deger);

function YatirimSayfasi() {
  const [kurlar, setKurlar] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [secilenKod, setSecilenKod] = useState('USD');
  const [miktar, setMiktar] = useState('100');

  useEffect(() => {
    getDovizKurlari()
      .then(setKurlar)
      .catch(() => setError('Döviz kurları alınamadı. Backend çalışıyor mu kontrol edin.'))
      .finally(() => setLoading(false));
  }, []);

  const secilenKur = kurlar.find((k) => k.kod === secilenKod);
  const sonuc = secilenKur ? Number(miktar || 0) * secilenKur.tlKarsiligi : 0;

  return (
    <div className="yatirim-sayfasi">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Yatırım</div>
          <h1 className="page-header__title">Döviz Kurları</h1>
        </div>
      </div>

      {loading && <div className="state-message">Kurlar yükleniyor...</div>}
      {error && <div className="state-message state-message--error">{error}</div>}

      {!loading && !error && (
        <>
          <div className="doviz-grid">
            {kurlar.map((kur) => (
              <div key={kur.kod} className="doviz-card">
                <div className="doviz-card__top">
                  <span className="doviz-card__sembol">{kur.sembol}</span>
                  <span className="doviz-card__kod">{kur.kod}</span>
                </div>
                <div className="doviz-card__deger">{formatTl(kur.tlKarsiligi)}</div>
                <div className="doviz-card__ad">{kur.ad}</div>
              </div>
            ))}
          </div>

          <div className="section-header">
            <h2 className="section-header__title">Hızlı Çevirici</h2>
          </div>

          <div className="form-card converter-card">
            <div className="converter-row">
              <div className="form-field">
                <label htmlFor="miktar">Miktar</label>
                <input
                  id="miktar"
                  type="number"
                  min="0"
                  value={miktar}
                  onChange={(e) => setMiktar(e.target.value)}
                />
              </div>

              <div className="form-field">
                <label htmlFor="birim">Birim</label>
                <select
                  id="birim"
                  value={secilenKod}
                  onChange={(e) => setSecilenKod(e.target.value)}
                >
                  {kurlar.map((kur) => (
                    <option key={kur.kod} value={kur.kod}>
                      {kur.kod}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="converter-result">
              <span className="converter-result__label">Türk Lirası Karşılığı</span>
              <span className="converter-result__value">{formatTl(sonuc)}</span>
            </div>
          </div>
        </>
      )}
    </div>
  );
}

export default YatirimSayfasi;