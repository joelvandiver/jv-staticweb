import * as React from "react"
import { Link, graphql } from "gatsby"

import Bio from "../components/bio"
import Layout from "../components/layout"
import SEO from "../components/seo"

const loadAPOD = () => {
    const date = new Date();
    const today = date.toISOString().split("T")[0]
    const key = "APOD_RESULT";
    const last = localStorage.getItem(key);
    const lastJson = last ? JSON.parse(last) : undefined;

    function renderImg(json) {
        // Fallback on Jupiter if the media_type is not an image.
        var src = json.media_type === "image" ?
            json.url :
            "https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pia22949.jpg";
        var container = document.getElementById("nasa-img-container");
        if (!container) {
            console.log("#nasa-img-container is missing!");
            return;
        }
        if (container.childElementCount) { return; }
        var img = document.createElement('img');
        img.src = src;
        container.title = json.explanation;
        container.appendChild(img);
        localStorage.setItem(key, JSON.stringify(json));
    }

    if (!lastJson || lastJson.date !== today) {
        fetch("https://api.nasa.gov/planetary/apod?api_key=5VhLWB6JaWs6m11mLfrNKply8naxchfbWb0Nu2Q9")
            .then((response) => response.json())
            .then(renderImg);
    } else {
        renderImg(lastJson);
    }
}

const BlogPostTemplate = ({ data, location }) => {
    const post = data.markdownRemark
    const siteTitle = data.site.siteMetadata?.title || `Title`
    const { previous, next } = data

    loadAPOD();

    return (
        <Layout location={location} title={siteTitle}>
            <SEO
                title={post.frontmatter.title}
                description={post.frontmatter.description || post.excerpt}
            />
            <article
                className="blog-post"
                itemScope
                itemType="http://schema.org/Article"
            >
                <header>
                    <h1 itemProp="headline">{post.frontmatter.title}</h1>
                    <p>{post.frontmatter.date}</p>
                </header>
                <section>
                    <div id="nasa-img-container"></div>
                </section>
                <section
                    dangerouslySetInnerHTML={{ __html: post.html }}
                    itemProp="articleBody"
                />
                <hr />
                <footer>
                    <Bio />
                </footer>
            </article>
            <nav className="blog-post-nav">
                <ul
                    style={{
                        display: `flex`,
                        flexWrap: `wrap`,
                        justifyContent: `space-between`,
                        listStyle: `none`,
                        padding: 0,
                    }}
                >
                    <li>
                        {previous && (
                            <Link to={previous.fields.slug} rel="prev">
                                ← {previous.frontmatter.title}
                            </Link>
                        )}
                    </li>
                    <li>
                        {next && (
                            <Link to={next.fields.slug} rel="next">
                                {next.frontmatter.title} →
                            </Link>
                        )}
                    </li>
                </ul>
            </nav>
        </Layout>
    )
}

export default BlogPostTemplate

export const pageQuery = graphql`
  query BlogPostBySlug(
    $id: String!
    $previousPostId: String
    $nextPostId: String
  ) {
    site {
      siteMetadata {
        title
      }
    }
    markdownRemark(id: { eq: $id }) {
      id
      excerpt(pruneLength: 160)
      html
      frontmatter {
        title
        date(formatString: "MMMM DD, YYYY")
        description
      }
    }
    previous: markdownRemark(id: { eq: $previousPostId }) {
      fields {
        slug
      }
      frontmatter {
        title
      }
    }
    next: markdownRemark(id: { eq: $nextPostId }) {
      fields {
        slug
      }
      frontmatter {
        title
      }
    }
  }
`
